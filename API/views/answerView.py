from django.utils.decorators import method_decorator
from django_ratelimit.decorators import ratelimit
from rest_framework import viewsets

from ..models import Answer
from ..serializers import AnswerSerializer


@method_decorator(ratelimit(key='ip', rate='2/s', method='GET', block=True), name='dispatch')
class AnswerView(viewsets.ModelViewSet):
    queryset = Answer.objects.all()
    serializer_class = AnswerSerializer
    http_method_names = ['get']