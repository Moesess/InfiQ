from django.conf import settings
from django.conf.urls.static import static
from django.contrib import admin
from django.urls import path, include
from rest_framework.routers import DefaultRouter
from django.contrib.auth import views as auth_views

from API import views

router = DefaultRouter()
router.register(r'Answers', views.AnswerView)
router.register(r'Questions', views.QuestionView)
router.register(r'Tests', views.TestView)
router.register(r'TestResults', views.TestResultView)
router.register(r'TestTypes', views.TestTypeView)
router.register(r'Users', views.UserView)

urlpatterns = [
    path('api-auth/', include('rest_framework.urls', namespace='rest_framework')),
    path('admin/', admin.site.urls),
    path('', include(router.urls)),
    path('accounts/', include('allauth.urls')),
    path('login/', views.LoginView, name='login'),
    path('logout/', auth_views.LogoutView.as_view(), name='logout'),
]

urlpatterns += static(settings.MEDIA_URL, document_root=settings.MEDIA_ROOT)
