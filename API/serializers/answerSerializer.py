from rest_framework import serializers
from API.models import Answer


class AnswerSerializer(serializers.ModelSerializer):
    class Meta:
        model = Answer
        fields = ['a_uid', 'a_text', 'a_question', 'a_correct']
