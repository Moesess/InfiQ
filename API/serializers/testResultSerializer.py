from rest_framework import serializers
from API.models import TestResult
from API.serializers.fields import CustomDateTimeField


class TestResultSerializer(serializers.ModelSerializer):
    tr_date_start = CustomDateTimeField()
    tr_date_end = CustomDateTimeField()

    class Meta:
        model = TestResult
        fields = ['tr_uid', 'tr_user', 'tr_test', 'tr_date_start', 'tr_date_end', 'tr_score']
