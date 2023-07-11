from rest_framework import serializers
from API.models import Question, Answer
from API.serializers import AnswerSerializer
from API.serializers.fields import CustomDateTimeField


class QuestionSerializer(serializers.ModelSerializer):
    uid = serializers.UUIDField(source="q_uid", read_only=True)
    id = serializers.CharField(source='q_id')
    testType = serializers.CharField(source='q_testType')
    text = serializers.CharField(source='q_text')
    img = serializers.ImageField(source='q_img')
    created_at = CustomDateTimeField(source='q_created_at', read_only=True)
    answers = serializers.SerializerMethodField()

    @staticmethod
    def get_answers(obj):
        answers = Answer.objects.filter(a_question=obj)
        return AnswerSerializer(answers, many=True).data

    class Meta:
        model = Question
        fields = ['uid', 'id', 'testType', 'text', 'img', 'created_at', 'answers']
