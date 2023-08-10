from django.contrib.auth.models import User
from django.db import transaction
from django.utils import timezone
from rest_framework import viewsets, status
from rest_framework.decorators import action
from rest_framework.response import Response
from rest_framework.status import HTTP_401_UNAUTHORIZED

from ..models import Test, Question, Answer, TestType, TestResult
from ..serializers import TestSerializer, RandomQuestionAnswerSerializer, AnswerSerializer, \
    TestResultSerializer, TestValidateSerializer


class TestView(viewsets.ModelViewSet):
    queryset = Test.objects.all().prefetch_related('testresult_set')
    serializer_class = TestSerializer

    @transaction.atomic
    @action(methods=['post'], detail=False, url_path='random_question')
    def random_question(self, request) -> Response:
        # Znajdź typ testu
        testType = TestType.objects.get(tt_uid=request.data['testType'])

        # Utwórz nowy test
        test = Test.objects.create(t_testType=testType, t_user=request.user)

        # Dodaj do testu jedno randomowe pytanie
        question = Question.objects.filter(q_testType__tt_name=testType).order_by('?').first()
        test.t_questions.add(question)

        return Response(TestSerializer(test).data, status=status.HTTP_200_OK)

    @transaction.atomic
    @action(detail=False, methods=['post'], url_path='random_question_answer',
            serializer_class=RandomQuestionAnswerSerializer)
    def random_question_answer(self, request) -> Response:
        # Utwórz serializer na podstawie requesta i wypełnij danymi
        serializer = RandomQuestionAnswerSerializer(data=request.data)
        serializer.is_valid(raise_exception=True)
        question_uid = serializer.validated_data.get('question_uid')
        answer_uid = serializer.validated_data.get('answer_uid')
        test_uid = serializer.validated_data.get('test_uid')

        # Pobierz odpowiedni TestResult, który będziemy wypełniać
        testResult: TestResult = TestResult.objects.get(tr_test__t_uid=test_uid)
        if testResult.tr_isDone:
            return Response(
                {'error': 'Test został już wykonany!.'},
                status=status.HTTP_403_FORBIDDEN
            )

        # Sprawdzanie odpowiedzi
        submitted_answer = Answer.objects.get(a_uid=answer_uid)
        answer = Answer.objects.get(a_question__q_uid=question_uid, a_correct=True)

        # Wypełnienie testu i zapis do bazy
        testResult.tr_isDone = True
        testResult.tr_score = int(submitted_answer.a_correct)
        testResult.tr_date_end = timezone.now()
        testResult.save()

        return Response(
            {
                'is_correct': submitted_answer.a_correct,
                'test_result': TestResultSerializer(testResult).data,
                'correct_answer': AnswerSerializer(answer).data
            },
            status=status.HTTP_200_OK
        )

    @transaction.atomic
    @action(methods=['post'], detail=False, url_path='random_40_question')
    def random_40_question(self, request) -> Response:
        # TODO ONLY FOR DEBUG
        if request.user.is_anonymous:
            try:
                user = User.objects.get(username='user1')
            except:
                return Response({"detail": "Authentication credentials were not provided."},
                                status=HTTP_401_UNAUTHORIZED)
        else:
            user = request.user

        # Znajdź typ testu
        testType = TestType.objects.get(tt_uid=request.data['testType'])

        # Utwórz nowy test
        test = Test.objects.create(t_testType=testType, t_user=user)

        # Dodaj do testu jedno randomowe pytanie
        questions = Question.objects.filter(q_testType__tt_name=testType).order_by('?')[:5]
        test.t_questions.set(questions)

        return Response(TestSerializer(test).data, status=status.HTTP_200_OK)

    @transaction.atomic
    @action(methods=['post'], detail=False, url_path='test_validate',
            serializer_class=TestValidateSerializer)
    def test_validate(self, request) -> Response:
        # Utwórz serializator na podstawie żądania i wypełnij danymi
        serializer = TestValidateSerializer(data=request.data)
        serializer.is_valid(raise_exception=True)

        # Pobierz uid testu i odpowiedzi użytkownika
        test_uid = serializer.validated_data.get('test_uid')
        user_answers = serializer.validated_data.get('answers')

        # Pobierz test na podstawie uid i pytania do testu
        test = Test.objects.get(t_uid=test_uid)

        score = 0
        # Sprawdzanie odpowiedzi i zliczanie punktów
        for question in test.t_questions.all():
            correct_answer = question.answers.filter(a_correct=True).first()
            user_answer = Answer.objects.filter(a_uid=user_answers[str(question.q_uid)]).first()

            if user_answer and correct_answer and user_answer.a_uid == correct_answer.a_uid:
                score += 1

        # Wypełnienie końcowego rezultatu
        with transaction.atomic():
            testResult = TestResult.objects.select_related('tr_test').get(tr_test=test)
            testResult.tr_isDone = True
            testResult.tr_score = score
            testResult.tr_date_end = timezone.now()
            testResult.save()

        testResultSerializer = TestResultSerializer(testResult)

        return Response({'test_result': testResultSerializer.data}, status=status.HTTP_200_OK)
