from rest_framework import serializers
from API.models import ExamType


class ExamTypeSerializer(serializers.ModelSerializer):
    class Meta:
        model = ExamType
        fields = ['e_uid', 'e_name', 'e_text']
