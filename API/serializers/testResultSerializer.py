from rest_framework import serializers

from API.models import TestResult
from API.serializers.fields import CustomDateTimeField


class TestResultSerializer(serializers.ModelSerializer):
    uid = serializers.UUIDField(source='tr_uid', read_only=True)
    user = serializers.StringRelatedField(source='tr_user')
    test = serializers.StringRelatedField(source='tr_test')
    score = serializers.CharField(source='tr_score')
    date_start = CustomDateTimeField(source='tr_date_start')
    date_end = CustomDateTimeField(source='tr_date_end')
    isDone = serializers.BooleanField(source='tr_isDone')

    class Meta:
        model = TestResult
        fields = ['uid', 'user', 'test', 'isDone', 'score', 'date_start', 'date_end']
