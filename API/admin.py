from django.contrib import admin

from .models import TestType, Question, Answer, Test, TestResult, User


# Register your models here.

@admin.register(TestType)
class TestTypeAdmin(admin.ModelAdmin):
    list_display = ('tt_uid', 'tt_name', 'tt_text')


@admin.register(Question)
class QuestionAdmin(admin.ModelAdmin):
    list_display = ('q_uid', 'q_testType', 'q_text')


@admin.register(Answer)
class AnswerAdmin(admin.ModelAdmin):
    list_display = ('a_uid', 'a_text', 'a_question', 'a_correct')


@admin.register(Test)
class TestAdmin(admin.ModelAdmin):
    list_display = ('t_uid', 't_testType', 't_user')
    filter_horizontal = ('t_questions',)


@admin.register(TestResult)
class TestResultAdmin(admin.ModelAdmin):
    list_display = ('tr_uid', 'tr_test', 'tr_date_start', 'tr_date_end', 'tr_score')


@admin.register(User)
class UserAdmin(admin.ModelAdmin):
    list_display = ('username', 'u_number_of_tests', 'u_best_score',
                    'u_best_time', 'u_correct_answers', 'u_all_answers')
