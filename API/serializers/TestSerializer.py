from rest_framework import serializers
from API.models import Test


class TestSerializer(serializers.ModelSerializer):
    class Meta:
        model = Test
        fields = ['t_uid', 't_testType', 't_questions']
