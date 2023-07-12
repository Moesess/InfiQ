from rest_framework import serializers

from API.models import TestType
from API.serializers.fields import CustomDateTimeField


class TestTypeSerializer(serializers.ModelSerializer):
    uid = serializers.UUIDField(source='tt_uid', read_only=True)
    name = serializers.CharField(source='tt_name')
    text = serializers.CharField(source='tt_text')
    created_at = CustomDateTimeField(source='tt_created_at', read_only=True)

    class Meta:
        model = TestType
        fields = ['uid', 'name', 'text', 'created_at']
