# Generated by Django 4.2.3 on 2023-08-23 19:29

from django.conf import settings
import django.contrib.auth.models
import django.contrib.auth.validators
from django.db import migrations, models
import django.db.models.deletion
import django.utils.timezone
import uuid


class Migration(migrations.Migration):

    initial = True

    dependencies = [
        ('auth', '0012_alter_user_first_name_max_length'),
    ]

    operations = [
        migrations.CreateModel(
            name='Question',
            fields=[
                ('q_uid', models.UUIDField(default=uuid.uuid4, editable=False, primary_key=True, serialize=False, unique=True, verbose_name='UID')),
                ('q_text', models.TextField(verbose_name='Text')),
                ('q_img', models.ImageField(blank=True, null=True, upload_to='')),
                ('q_created_at', models.DateTimeField(default=django.utils.timezone.now)),
                ('q_id', models.CharField(blank=True, editable=False, max_length=255, unique=True)),
            ],
            options={
                'verbose_name': 'Question',
                'ordering': ['q_uid'],
                'get_latest_by': ['q_uid'],
            },
        ),
        migrations.CreateModel(
            name='Test',
            fields=[
                ('t_uid', models.UUIDField(default=uuid.uuid4, editable=False, primary_key=True, serialize=False, unique=True, verbose_name='UID')),
                ('t_created_at', models.DateTimeField(default=django.utils.timezone.now)),
                ('t_questions', models.ManyToManyField(to='API.question', verbose_name='Questions')),
            ],
            options={
                'verbose_name': 'Test',
                'ordering': ['t_uid'],
                'get_latest_by': ['t_uid'],
            },
        ),
        migrations.CreateModel(
            name='TestType',
            fields=[
                ('tt_uid', models.UUIDField(default=uuid.uuid4, editable=False, primary_key=True, serialize=False, unique=True, verbose_name='UID')),
                ('tt_name', models.CharField(max_length=20, unique=True, verbose_name='Name')),
                ('tt_text', models.TextField(verbose_name='Text')),
                ('tt_created_at', models.DateTimeField(default=django.utils.timezone.now)),
            ],
            options={
                'verbose_name': 'ExamType',
                'ordering': ['tt_uid'],
                'get_latest_by': ['tt_uid'],
            },
        ),
        migrations.CreateModel(
            name='User',
            fields=[
                ('id', models.BigAutoField(auto_created=True, primary_key=True, serialize=False, verbose_name='ID')),
                ('password', models.CharField(max_length=128, verbose_name='password')),
                ('last_login', models.DateTimeField(blank=True, null=True, verbose_name='last login')),
                ('is_superuser', models.BooleanField(default=False, help_text='Designates that this user has all permissions without explicitly assigning them.', verbose_name='superuser status')),
                ('username', models.CharField(error_messages={'unique': 'A user with that username already exists.'}, help_text='Required. 150 characters or fewer. Letters, digits and @/./+/-/_ only.', max_length=150, unique=True, validators=[django.contrib.auth.validators.UnicodeUsernameValidator()], verbose_name='username')),
                ('first_name', models.CharField(blank=True, max_length=150, verbose_name='first name')),
                ('last_name', models.CharField(blank=True, max_length=150, verbose_name='last name')),
                ('email', models.EmailField(blank=True, max_length=254, verbose_name='email address')),
                ('is_staff', models.BooleanField(default=False, help_text='Designates whether the user can log into this admin site.', verbose_name='staff status')),
                ('is_active', models.BooleanField(default=True, help_text='Designates whether this user should be treated as active. Unselect this instead of deleting accounts.', verbose_name='active')),
                ('date_joined', models.DateTimeField(default=django.utils.timezone.now, verbose_name='date joined')),
                ('u_number_of_tests', models.PositiveIntegerField(default=0, verbose_name='Number of tests')),
                ('u_best_score', models.IntegerField(default=0, verbose_name='Best score')),
                ('u_best_time', models.DurationField(verbose_name='Best time')),
                ('u_correct_answers', models.PositiveIntegerField(default=0, verbose_name='Correct answers')),
                ('u_all_answers', models.PositiveIntegerField(default=0, verbose_name='All answers')),
                ('groups', models.ManyToManyField(blank=True, help_text='The groups this user belongs to. A user will get all permissions granted to each of their groups.', related_name='user_set', related_query_name='user', to='auth.group', verbose_name='groups')),
                ('user_permissions', models.ManyToManyField(blank=True, help_text='Specific permissions for this user.', related_name='user_set', related_query_name='user', to='auth.permission', verbose_name='user permissions')),
            ],
            options={
                'verbose_name': 'User',
                'ordering': ['username'],
                'get_latest_by': ['username'],
            },
            managers=[
                ('objects', django.contrib.auth.models.UserManager()),
            ],
        ),
        migrations.CreateModel(
            name='TestResult',
            fields=[
                ('tr_uid', models.UUIDField(default=uuid.uuid4, editable=False, primary_key=True, serialize=False, unique=True, verbose_name='UID')),
                ('tr_date_start', models.DateTimeField(default=django.utils.timezone.now, verbose_name='Start date')),
                ('tr_date_end', models.DateTimeField(null=True, verbose_name='End date')),
                ('tr_score', models.IntegerField(verbose_name='Score')),
                ('tr_isDone', models.BooleanField(default=False)),
                ('tr_test', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='API.test', verbose_name='Test')),
            ],
            options={
                'verbose_name': 'TestResult',
                'ordering': ['tr_uid'],
                'get_latest_by': ['tr_uid'],
            },
        ),
        migrations.AddField(
            model_name='test',
            name='t_testType',
            field=models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='API.testtype', verbose_name='Test type'),
        ),
        migrations.AddField(
            model_name='test',
            name='t_user',
            field=models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to=settings.AUTH_USER_MODEL, verbose_name='User'),
        ),
        migrations.AddField(
            model_name='question',
            name='q_testType',
            field=models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, to='API.testtype', verbose_name='Test type'),
        ),
        migrations.CreateModel(
            name='Answer',
            fields=[
                ('a_uid', models.UUIDField(default=uuid.uuid4, editable=False, primary_key=True, serialize=False, unique=True, verbose_name='UID')),
                ('a_text', models.TextField(verbose_name='Text')),
                ('a_correct', models.BooleanField(verbose_name='Is correct?')),
                ('a_created_at', models.DateTimeField(default=django.utils.timezone.now)),
                ('a_question', models.ForeignKey(on_delete=django.db.models.deletion.CASCADE, related_name='answers', to='API.question', verbose_name='Question')),
            ],
            options={
                'verbose_name': 'Answer',
                'ordering': ['a_uid'],
                'get_latest_by': ['a_uid'],
            },
        ),
    ]
