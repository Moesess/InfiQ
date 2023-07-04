from rest_framework import serializers
from API.models import Question


class QuestionSerializer(serializers.ModelSerializer):
    class Meta:
        model = Question
        fields = ['q_uid', 'q_exam', 'q_text']
