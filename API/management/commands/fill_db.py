from io import BytesIO

import requests
from bs4 import BeautifulSoup
from django.core.files.base import ContentFile
from django.core.management.base import BaseCommand

from PIL import Image

from API.models import TestType, Question, Answer, TEST_TYPES


def resize_image(image, desired_width, desired_height):
    # Calculate the aspect ratio of the image and the desired size
    img_aspect = image.width / image.height
    desired_aspect = desired_width / desired_height

    # Determine the dimensions to resize the image to while maintaining its aspect ratio
    if img_aspect > desired_aspect:
        new_width = desired_width
        new_height = int(desired_width / img_aspect)
    else:
        new_height = desired_height
        new_width = int(desired_height * img_aspect)

    # Create a new image with the desired size and a white background
    new_image = Image.new("RGB", (desired_width, desired_height), "white")
    # Paste the resized image onto the center of the new image
    new_image.paste(image.resize((new_width, new_height)),
                    ((desired_width - new_width) // 2, (desired_height - new_height) // 2))

    return new_image


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

        if response.status_code == 200:
            self.stdout.write(self.style.SUCCESS("Connected to website"))
            soup = BeautifulSoup(response.content, 'html.parser')

            question_divs = soup.find_all('div', class_='question')
            self.stdout.write(self.style.SUCCESS("Started scrapping"))

            for question_div in question_divs:
                question_title = question_div.find('div', class_='title').text.strip().split('. ', 1)[1].capitalize()

                question = Question.objects.create(q_testType=test_type, q_text=question_title)
                self.stdout.write(self.style.SUCCESS(f"Added question {question.q_id}"))
                answer_divs = question_div.find_all('div', class_='answer')

                for answer_div in answer_divs:
                    answer_text = answer_div.text.strip()[3:]

                    if 'correct' in answer_div['class']:
                        correct = True
                    else:
                        correct = False

                    answer = Answer.objects.create(a_question=question, a_text=answer_text, a_correct=correct)
                    self.stdout.write(self.style.SUCCESS(f"Added answer {answer.a_uid}"))

                image_div = question_div.find('div', class_='image')
                if image_div:
                    try:
                        image_url = image_div.find('img')['src']
                        image_response = requests.get(url + image_url)

                        if image_response.status_code == 200:
                            # Open the image using Pillow
                            image = Image.open(ContentFile(image_response.content))

                            # Resize the image
                            resized_image = resize_image(image, 720, 450)

                            # Convert the resized image back to a file-like object
                            image_io = BytesIO()
                            resized_image.save(image_io, format='JPEG')
                            image_file = ContentFile(image_io.getvalue())

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
