function confirmSubmit()
{
    if (confirm('Are you sure you want to submit these changes?'))
    {
        document.getElementById('submissionForm').submit()
    }
}
