from rest_framework import serializers

from API.models import User
from API.serializers.fields import CustomDateTimeField

class UserSerializer(serializers.ModelSerializer):
    name = serializers.CharField(source='name')
    email =  serializers.CharField(source='email')
    password = serializers.CharField(source='password')
    number_of_tests = serializers.IntegerField(source='number_of_tests')
    best_score = serializers.IntegerField(source='best_score')
    best_time = serializers.DateTimeField(source='best_time')
    correct_answers = serializers.IntegerField(source='correct_answers')
    all_answers = serializers.IntegerField(source='all_answers')
    accuracy = serializers.FloatField(source='accuracy')