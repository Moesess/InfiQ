from django.core.management import BaseCommand

from API.models import TestType, Question, Answer, Test
import uuid


class Command(BaseCommand):
    help = 'Seed Database with random data'

    def handle(self, *args, **options):
        self.stdout.write("Seeding test types...")

        for i in range(1, 5):
            test_type = TestType.objects.create(
                tt_uid=uuid.uuid4(),
                tt_name=f'INF.0{i}',
                tt_text=f'Informational Test 0{i}'
            )

            create_test(test_type)


def create_test(test_type: TestType) -> None:
    test = Test.objects.create(
        t_uid=uuid.uuid4(),
        t_testType=test_type
    )

    for i in range(1, 41):
        test.t_questions.add(create_question_with_answers(i, test_type))
    return


def create_question_with_answers(question_number: int, test_type: TestType) -> Question:
    question = Question.objects.create(
        q_uid=uuid.uuid4(),
        q_testType=test_type,
        q_text=f'This is a question {question_number}'
    )

    for i in range(1, 5):
        Answer.objects.create(
            a_uid=uuid.uuid4(),
            a_text=f'Answer of {question.q_text}.',
            a_question=question,
            a_correct=True if i != 4 else False
        )

    return question
