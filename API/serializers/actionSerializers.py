from rest_framework import serializers


class RandomQuestionAnswerSerializer(serializers.Serializer):
    test_uid = serializers.UUIDField()
    question_uid = serializers.UUIDField()
    answer_uid = serializers.UUIDField()
