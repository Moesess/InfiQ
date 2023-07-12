from rest_framework import serializers

from API.models import Test, TestType, TestResult
from API.serializers import QuestionSerializer, TestResultSerializer
from API.serializers.fields import CustomDateTimeField


class TestSerializer(serializers.ModelSerializer):
    uid = serializers.UUIDField(source='t_uid', read_only=True)
    testType = serializers.PrimaryKeyRelatedField(source='t_testType', queryset=TestType.objects.all())
    questions = QuestionSerializer(source='t_questions', many=True, read_only=True)
    testResult = serializers.SerializerMethodField()
    created_at = CustomDateTimeField(source='t_created_at', read_only=True)

    @staticmethod
    def get_testResult(obj):
        testResult = TestResult.objects.get(tr_test=obj)
        return TestResultSerializer(testResult).data

    class Meta:
        model = Test
        fields = ['uid', 'testType', 'questions', 'testResult', 'created_at']
