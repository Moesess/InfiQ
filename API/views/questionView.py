from django.utils.decorators import method_decorator
from django_ratelimit.decorators import ratelimit
from rest_framework import viewsets

from ..models import Question
from ..serializers import QuestionSerializer


@method_decorator(ratelimit(key='ip', rate='2/s', method='GET', block=True), name='dispatch')
class QuestionView(viewsets.ModelViewSet):
    queryset = Question.objects.all().prefetch_related('answers')
    serializer_class = QuestionSerializer
    http_method_names = ['get']