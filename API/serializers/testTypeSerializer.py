from rest_framework import serializers
from API.models import TestType


class TestTypeSerializer(serializers.ModelSerializer):
    class Meta:
        model = TestType
        fields = ['tt_uid', 'tt_name', 'tt_text']
