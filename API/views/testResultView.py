from django.utils.decorators import method_decorator
from django_ratelimit.decorators import ratelimit
from rest_framework import viewsets

from ..models import TestResult
from ..serializers import TestResultSerializer


@method_decorator(ratelimit(key='ip', rate='2/s', method='GET', block=True), name='dispatch')
class TestResultView(viewsets.ModelViewSet):
    queryset = TestResult.objects.all()
    serializer_class = TestResultSerializer
    http_method_names = ['get']