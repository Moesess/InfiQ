from django.test import TestCase, Client
from django.urls import reverse
from API.models import Test, Question, Answer
from django.contrib.auth.models import User

import uuid


class TestAPIView(TestCase):
    def setUp(self):
        # Setup your test database if necessary
        self.user = User.objects.create_user(username='testuser', password='testpass')
        self.test = Test.objects.create(t_user=self.user)
        self.question = Question.objects.create(q_testType=self.test.t_testType)
        self.answer = Answer.objects.create(a_question=self.question)

    def test_validate_test(self):
        # Instantiate the test client
        client = Client()

        # Log in the test client
        client.login(username='testuser', password='testuser')

        # Prepare the data for the POST request
        data = {
            'test_uid': str(self.test.t_uid),
            'answers': {
                str(self.question.q_uid): str(self.answer.a_uid),
            },
        }

        # Make a POST request to the endpoint
        response = client.post(reverse('test_validate'), data, content_type='application/json')

        # Check the response status code
        self.assertEqual(response.status_code, 200)

        # Check the response data (you may need to adjust this depending on what your view returns)
        self.assertEqual(response.json(), {
            'is_correct': True,
            'test_result': {...},  # Expected test_result
            'correct_answer': {...},  # Expected correct_answer
        })
