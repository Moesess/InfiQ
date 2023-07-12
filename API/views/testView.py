import datetime

from django.core.exceptions import ObjectDoesNotExist
from rest_framework import viewsets, status
from rest_framework.decorators import action
from rest_framework.response import Response

from ..models import Test, Question, Answer, TestType, TestResult
from ..serializers import TestSerializer, SubmitAnswerSerializer, AnswerSerializer, \
    TestResultSerializer


class TestView(viewsets.ModelViewSet):
    queryset = Test.objects.all()
    serializer_class = TestSerializer

    @action(methods=['post'], detail=False, url_path='random_question')
    def random_question(self, request):
        """
        :param request: Funkcja pobiera dane od użytkownika, jaki rodzaj pytania chce otrzymać.
        :description: Musi sprawdzić jaki rodzaj testu jest potrzebny,
                      utworzy test, doda do niego pytanie i zwróci zserializowany test
        :return:
        """
        testType = TestType.objects.filter(tt_uid=request.data['testType']).first()
        test = Test.objects.create(t_testType=testType, t_user=request.user)
        question = Question.objects.order_by('?').first()
        test.t_questions.add(question)
        self.queryset = question
        serializer = TestSerializer(test)

        return Response(serializer.data, status=status.HTTP_200_OK)

    @action(detail=False, methods=['post'], url_path='random_question_answer', serializer_class=SubmitAnswerSerializer)
    def random_question_answer(self, request):
        serializer = SubmitAnswerSerializer(data=request.data)
        serializer.is_valid(raise_exception=True)
        question_uid = serializer.validated_data.get('question_uid')
        answer_uid = serializer.validated_data.get('answer_uid')
        test_uid = serializer.validated_data.get('test_uid')

        try:
            testResult: TestResult = TestResult.objects.get(tr_test__t_uid=test_uid)
            if testResult.tr_isDone:
                raise Exception("Test został już wykonany!")
            else:
                submitted_answer: Answer = Answer.objects.get(a_uid=answer_uid)
                answer: Answer = Answer.objects.get(a_question__q_uid=question_uid, a_correct=True)
                correct_answer: AnswerSerializer = AnswerSerializer(answer).data
                is_correct: bool = submitted_answer.a_correct

                testResult.tr_isDone = True
                testResult.tr_score = 1 if is_correct else 0
                testResult.tr_date_end = datetime.datetime.now()
                testResult.save()
                testResultSerializer = TestResultSerializer(testResult)

        except ObjectDoesNotExist:
            return Response({'error': 'Invalid question_uid or answer_uid.'}, status=status.HTTP_400_BAD_REQUEST)

        return Response({'is_correct': is_correct, 'test_result': testResultSerializer.data,
                         'correct_answer': correct_answer}, status=status.HTTP_200_OK)
