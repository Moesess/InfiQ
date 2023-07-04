from rest_framework import viewsets

from ..models import TestType
from ..serializers import TestTypeSerializer


class TestTypeView(viewsets.ModelViewSet):
    queryset = TestType.objects.all()
    serializer_class = TestTypeSerializer

