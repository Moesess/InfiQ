from django.utils import timezone
from rest_framework import status
from rest_framework.response import Response
from rest_framework.views import APIView

from API.models import Question, Answer, Test, TestResult
from API.serializers import QuestionSerializer, AnswerSerializer, TestSerializer, TestResultSerializer


class StartQuestionView(APIView):
    def get(self, request):
        question = Question.objects.order_by('?').first()
        serializer = QuestionSerializer(question)
        return Response(serializer.data, status=status.HTTP_200_OK)


class SubmitAnswerView(APIView):
    def post(self, request):
        question_uid: str = request.data.get('question_uid')
        submitted_answer_text: str = request.data.get('answer')

        question: Question = Question.objects.get(q_uid=question_uid)

        # Check if answer is correct
        is_correct: bool = Answer.objects.filter(
            a_question=question, a_text=submitted_answer_text, a_correct=True).exists()

        # Find correct answer and serialize it
        correct_answer: AnswerSerializer = AnswerSerializer(
            Answer.objects.filter(a_question=question, a_correct=True).first())

        return Response({'is_correct': is_correct, 'correct_answer': correct_answer.data}, status=status.HTTP_200_OK)


class StartTestView(APIView):
    def post(self, request):
        # Create a new Test instance and select the questions
        test = Test.objects.create(...)
        questions = Question.objects.order_by('?')[:40]
        test.t_questions.set(questions)

        # Create a new TestResult instance
        TestResult.objects.create(tr_user=request.user, tr_test=test, tr_date_start=timezone.now())

        # Serialize the test and return it in the response
        serializer = TestSerializer(test)
        return Response(serializer.data, status=status.HTTP_201_CREATED)


class EndTestView(APIView):
    def post(self, request, test_uid):
        # Get the Test instance and the user's answers from the request
        test = Test.objects.get(t_uid=test_uid)
        user_answers = request.data.get('answers')  # This should be a list of the user's answers

        # Check the user's answers and calculate the score
        score = 0
        for question, user_answer in zip(test.t_questions.all(), user_answers):
            if question.correct_answer == user_answer:
                score += 1

        # Update the TestResult instance
        test_result = TestResult.objects.get(tr_test=test, tr_user=request.user)
        test_result.tr_date_end = timezone.now()
        test_result.tr_score = score
        test_result.save()

        # Serialize the test result and return it in the response
        serializer = TestResultSerializer(test_result)
        return Response(serializer.data, status=status.HTTP_200_OK)
