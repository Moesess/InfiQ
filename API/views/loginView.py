from django.shortcuts import render


def LoginView(request):
    return render(request, 'rest_framework/login.html')
