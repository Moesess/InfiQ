from rest_framework.views import APIView
from rest_framework.response import Response
from rest_framework import status
from django.core.mail import send_mail


class SendEmailView(APIView):
    def post(self, request):
        subject = request.data.get('subject')
        user = request.data.get('username')
        message = f"{user}\n{request.data.get('message')}"
        recipient = 'infiq.report@gmail.com'

        try:
            send_mail(subject, message, recipient, [recipient])
        except Exception as e:
            return Response({'error': str(e)}, status=status.HTTP_500_INTERNAL_SERVER_ERROR)

        return Response({'status': 'Email sent successfully'}, status=status.HTTP_200_OK)
