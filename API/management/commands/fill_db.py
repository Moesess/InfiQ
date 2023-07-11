import requests
from bs4 import BeautifulSoup
from django.core.files.base import ContentFile
from django.core.management.base import BaseCommand

from API.models import TestType, Question, Answer, TEST_TYPES


class Command(BaseCommand):
    help = 'Fill db with exam questions'

    def handle(self, *args, **options):

        websites = {
            'INF.02': 'https://www.praktycznyegzamin.pl/ee08/teoria/wszystko/',
            'INF.03': 'https://www.praktycznyegzamin.pl/inf03ee09e14/teoria/wszystko/',
            'INF.04': 'https://www.praktycznyegzamin.pl/inf04/teoria/wszystko/',
        }

        for test_type, website in websites.items():
            try:
                self.scrape_questions(website, test_type)
                self.stdout.write(self.style.SUCCESS(f"Scrapped questions in {test_type}"))
            except Exception as inst:
                self.stdout.write(self.style.WARNING(inst))
                self.stdout.write(self.style.WARNING('ERROR Could not scrape questions'))

    def scrape_questions(self, url, test_type):
        response = requests.get(url)
        test_type = TestType.objects.create(tt_name=test_type, tt_text=TEST_TYPES[test_type])

        # TODO: WYWALIĆ NUMERKI PYTAŃ, ONE BĘDĄ GENEROWANE NASZĄ FUNKCJĄ W MODELU
        if response.status_code == 200:
            self.stdout.write(self.style.SUCCESS("Connected to website"))
            soup = BeautifulSoup(response.content, 'html.parser')

            question_divs = soup.find_all('div', class_='question')
            self.stdout.write(self.style.SUCCESS("Started scrapping"))
            # TODO: WYWALIĆ ZNACZNIKI ODPOWIEDZI A. B. C. D.
            for question_div in question_divs:
                question_title = question_div.find('div', class_='title').text.strip().split('. ', 1)[1].capitalize()

                question = Question.objects.create(q_testType=test_type, q_text=question_title)
                self.stdout.write(self.style.SUCCESS("Added question " + question.q_text))
                answer_divs = question_div.find_all('div', class_='answer')

                for answer_div in answer_divs:
                    answer_text = answer_div.text.strip()[2:]

                    if 'correct' in answer_div['class']:
                        correct = True
                    else:
                        correct = False

                    Answer.objects.create(a_question=question, a_text=answer_text, a_correct=correct)
                    self.stdout.write(self.style.SUCCESS("Added answer " + answer_text))

                image_div = question_div.find('div', class_='image')
                if image_div:
                    try:
                        image_url = image_div.find('img')['src']
                        image_response = requests.get(url + image_url)

                        if image_response.status_code == 200:
                            image_file = ContentFile(image_response.content)

                            image_file.name = 'question_' + str(question.q_uid) + '.jpg'

                            question.q_img.save(image_file.name, image_file)
                            question.save()

                    except TypeError as e:  # Usuń pytanie, jeśli nie udało się pobrać zdjęcia
                        self.stdout.write(self.style.WARNING(f'ERROR Could not scrape question {question.q_text}: {e}'))
                        question.delete()

                    except requests.exceptions.RequestException as e:
                        self.stdout.write(self.style.WARNING(f"An error occurred while fetching the image: {e}"))

        else:
            self.stdout.write(self.style.WARNING("Nie można pobrać strony"))
