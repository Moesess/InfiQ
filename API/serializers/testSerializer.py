from rest_framework import serializers
from API.models import Test
from API.serializers.fields import CustomDateTimeField


class TestSerializer(serializers.ModelSerializer):
    t_created_at = CustomDateTimeField()

    class Meta:
        model = Test
        fields = ['t_uid', 't_testType', 't_questions', 't_created_at']
