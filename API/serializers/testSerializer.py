from rest_framework import serializers
from API.models import Test, Question, TestType
from API.serializers import QuestionSerializer
from API.serializers.fields import CustomDateTimeField


class TestSerializer(serializers.ModelSerializer):
    uid = serializers.UUIDField(source='t_uid', read_only=True)
    testType = serializers.PrimaryKeyRelatedField(source='t_testType', queryset=TestType.objects.all())
    questions = QuestionSerializer(source='t_questions', many=True, read_only=True)
    created_at = CustomDateTimeField(source='t_created_at', read_only=True)

    class Meta:
        model = Test
        fields = ['uid', 'testType', 'questions', 'created_at']
