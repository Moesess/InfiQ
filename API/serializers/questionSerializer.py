from rest_framework import serializers
from API.models import Question
from API.serializers.fields import CustomDateTimeField


class QuestionSerializer(serializers.ModelSerializer):
    q_created_at = CustomDateTimeField()

    class Meta:
        model = Question
        fields = ['q_uid', 'q_id', 'q_testType', 'q_text', 'q_img', 'q_created_at']
