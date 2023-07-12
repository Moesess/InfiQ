import datetime

from django.core.exceptions import ObjectDoesNotExist
from rest_framework import viewsets, status
from rest_framework.decorators import action
from rest_framework.response import Response

from ..models import Test, Question, Answer, TestType, TestResult
from ..serializers import TestSerializer, RandomQuestionAnswerSerializer, AnswerSerializer, \
    TestResultSerializer


class TestView(viewsets.ModelViewSet):
    queryset = Test.objects.all()
    serializer_class = TestSerializer

    @action(methods=['post'], detail=False, url_path='random_question')
    def random_question(self, request):
        # Znajdź typ testu
        testType = TestType.objects.get(tt_uid=request.data['testType'])

        # Utwórz nowy test
        test = Test.objects.create(t_testType=testType, t_user=request.user)

        # Dodaj do testu jedno randomowe pytanie
        question = Question.objects.order_by('?').first()
        test.t_questions.add(question)

        return Response(TestSerializer(test).data, status=status.HTTP_200_OK)

    @action(detail=False, methods=['post'], url_path='random_question_answer',
            serializer_class=RandomQuestionAnswerSerializer)
    def random_question_answer(self, request):
        # Utwórz serializer na podstawie requesta i wypełnij danymi
        serializer = RandomQuestionAnswerSerializer(data=request.data)
        serializer.is_valid(raise_exception=True)
        question_uid = serializer.validated_data.get('question_uid')
        answer_uid = serializer.validated_data.get('answer_uid')
        test_uid = serializer.validated_data.get('test_uid')

        try:
            # Pobierz odpowiedni testResult, który będziemy wypełniać
            testResult: TestResult = TestResult.objects.get(tr_test__t_uid=test_uid)
            if testResult.tr_isDone:
                return Response(
                    {'error': 'Test został już wykonany!.'},
                    status=status.HTTP_403_FORBIDDEN
                )
            else:
                # Sprawdzanie odpowiedzi
                submitted_answer: Answer = Answer.objects.get(a_uid=answer_uid)
                answer: Answer = Answer.objects.get(a_question__q_uid=question_uid, a_correct=True)
                correct_answer: AnswerSerializer = AnswerSerializer(answer).data
                is_correct: bool = submitted_answer.a_correct

                # Wypełnienie testu i zapis do bazy
                testResult.tr_isDone = True
                testResult.tr_score = 1 if is_correct else 0
                testResult.tr_date_end = datetime.datetime.now()
                testResult.save()
                testResultSerializer = TestResultSerializer(testResult)

                return Response(
                    {'is_correct': is_correct,
                     'test_result': testResultSerializer.data,
                     'correct_answer': correct_answer},
                    status=status.HTTP_200_OK
                )

        except ObjectDoesNotExist:
            return Response(
                {'error': 'Invalid question_uid or answer_uid.'},
                status=status.HTTP_400_BAD_REQUEST
            )


