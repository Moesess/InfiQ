import requests
from bs4 import BeautifulSoup

from django.core.management.base import BaseCommand
from django.contrib.auth.models import User
from django.core.files.base import ContentFile
from django.core.files import File

from API.models import TestType, Question, Answer


class Command(BaseCommand):
    help = 'Fill db with exam questions'

    def handle(self, *args, **options):
        website = 'https://www.praktycznyegzamin.pl/ee08/teoria/wszystko/'
        try:
            self.scrape_questions(website)
        except Exception as inst:
            self.stdout.write(self.style.WARNING(inst))
            self.stdout.write(self.style.WARNING('ERROR Could not scrape questions'))

    def scrape_questions(self, url):
        response = requests.get(url)
        test_type = TestType.objects.create(tt_name='Test', tt_text='Test Exam')

        if response.status_code == 200:
            self.stdout.write(self.style.SUCCESS("Connected to website"))
            soup = BeautifulSoup(response.content, 'html.parser')

            question_divs = soup.find_all('div', class_='question')
            self.stdout.write(self.style.SUCCESS("Started scrapping"))
            for question_div in question_divs:
                question_title = question_div.find('div', class_='title').text.strip()

                question = Question.objects.create(q_testType=test_type, q_text=question_title)
                self.stdout.write(self.style.SUCCESS("Added question " + question.q_text))
                answer_divs = question_div.find_all('div', class_='answer')

                for answer_div in answer_divs:
                    answer_text = answer_div.text.strip()

                    if 'correct' in answer_div['class']:
                        correct = True
                    else:
                        correct = False

                    Answer.objects.create(a_question=question, a_text=answer_text, a_correct=correct)
                    self.stdout.write(self.style.SUCCESS("Added answer " + answer_text))

                image_div = question_div.find('div', class_='image')
                if image_div:
                    image_url = image_div.find('img')['src']
                    image_response = requests.get(url + image_url)

                    if image_response.status_code == 200:
                        image_file = ContentFile(image_response.content)

                        image_file.name = 'question_' + str(question.q_uid) + '.jpg'

                        question.q_img.save(image_file.name, image_file)
                        question.save()

        else:
            self.stdout.write(self.style.WARNING("Nie można pobrać strony"))
