from rest_framework import serializers
from API.models import Answer
from API.serializers.fields import CustomDateTimeField


class AnswerSerializer(serializers.ModelSerializer):
    uid = serializers.UUIDField(source='a_uid', read_only=True)
    text = serializers.CharField(source='a_text')
    created_at = CustomDateTimeField(source='a_created_at', read_only=True)
    question = serializers.CharField(source='a_question.q_uid')

    class Meta:
        model = Answer
        fields = ['uid', 'text', 'created_at', 'question']
