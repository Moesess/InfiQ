from django.core.exceptions import ObjectDoesNotExist
from rest_framework import viewsets, status
from rest_framework.decorators import action
from rest_framework.response import Response

from ..models import Question, Answer
from ..serializers import QuestionSerializer, AnswerSerializer
from ..serializers.actionSerializers import SubmitAnswerSerializer


class QuestionView(viewsets.ModelViewSet):
    queryset = Question.objects.all()
    serializer_class = QuestionSerializer

    @action(methods=['get'], detail=False, url_path='random_question')
    def random_question(self, request):
        question = Question.objects.order_by('?').first()
        self.queryset = question
        serializer = QuestionSerializer(question)
        return Response(serializer.data, status=status.HTTP_200_OK)

    @action(detail=False, methods=['post'], url_path='submit_answer', serializer_class=SubmitAnswerSerializer)
    def submit_answer(self, request):
        serializer = SubmitAnswerSerializer(data=request.data)
        serializer.is_valid(raise_exception=True)
        question_uid = serializer.validated_data.get('question_uid')
        answer_uid = serializer.validated_data.get('answer_uid')

        try:
            question: Question = Question.objects.get(q_uid=question_uid)
            submitted_answer: Answer = Answer.objects.get(a_uid=answer_uid)
        except ObjectDoesNotExist:
            return Response({'error': 'Invalid question_uid or answer_uid.'}, status=status.HTTP_400_BAD_REQUEST)

        is_correct: bool = submitted_answer.a_correct
        answer: Answer = Answer.objects.get(a_question__q_uid=question.q_uid, a_correct=True)
        correct_answer: AnswerSerializer = AnswerSerializer(answer).data
        return Response({'is_correct': is_correct, 'correct_answer': correct_answer}, status=status.HTTP_200_OK)

