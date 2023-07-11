from rest_framework import serializers
from API.models import Answer
from API.serializers.fields import CustomDateTimeField


class AnswerSerializer(serializers.ModelSerializer):
    a_created_at = CustomDateTimeField()

    class Meta:
        model = Answer
        fields = ['a_uid', 'a_text', 'a_question', 'a_correct', 'a_created_at']
