from django.utils.decorators import method_decorator
from django_ratelimit.decorators import ratelimit
from rest_framework import viewsets

from ..models import TestType
from ..serializers import TestTypeSerializer


@method_decorator(ratelimit(key='ip', rate='2/s', method='GET', block=True), name='dispatch')
class TestTypeView(viewsets.ModelViewSet):
    queryset = TestType.objects.all()
    serializer_class = TestTypeSerializer
    http_method_names = ['get']
