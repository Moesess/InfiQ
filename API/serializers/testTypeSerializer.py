from rest_framework import serializers
from API.models import TestType
from API.serializers.fields import CustomDateTimeField


class TestTypeSerializer(serializers.ModelSerializer):
    tt_created_at = CustomDateTimeField()

    class Meta:
        model = TestType
        fields = ['tt_uid', 'tt_name', 'tt_text', 'tt_created_at']
