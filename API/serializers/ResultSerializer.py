from rest_framework import serializers
from API.models import TestResult


class TestResultSerializer(serializers.ModelSerializer):
    class Meta:
        model = TestResult
        fields = ['r_uid', 'r_user', 'r_quiz', 't_date_start', 't_date_end', 't_score']

