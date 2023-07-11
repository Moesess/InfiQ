from django.conf import settings
from django.conf.urls.static import static
from django.contrib import admin
from django.urls import path, include
from rest_framework.routers import DefaultRouter

from API import views
from API.views import StartQuestionView, SubmitAnswerView

router = DefaultRouter()
router.register(r'Answers', views.AnswerView)
router.register(r'Questions', views.QuestionView)
router.register(r'Tests', views.TestView)
router.register(r'TestResults', views.TestResultView)
router.register(r'TestTypes', views.TestTypeView)

urlpatterns = [
    path('admin/', admin.site.urls),
    path('start-question/', StartQuestionView.as_view(), name='start-question'),
    path('submit-answer/', SubmitAnswerView.as_view(), name='submit-answer'),
    path('', include(router.urls)),
]

urlpatterns += static(settings.MEDIA_URL, document_root=settings.MEDIA_ROOT)
