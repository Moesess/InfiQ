from rest_framework import serializers
from API.models import TestResult


class TestResultSerializer(serializers.ModelSerializer):
    class Meta:
        model = TestResult
        fields = ['tr_uid', 'tr_user', 'tr_test', 'tr_date_start', 'tr_date_end', 'tr_score']

