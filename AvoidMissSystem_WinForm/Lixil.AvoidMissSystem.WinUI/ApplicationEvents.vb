Namespace My

    ' ���̃C�x���g�� MyApplication �ɑ΂��ė��p�ł��܂�:
    ' 
    ' Startup: �A�v���P�[�V�������J�n���ꂽ�Ƃ��A�X�^�[�g�A�b�v �t�H�[�����쐬�����O�ɔ������܂��B
    ' Shutdown: �A�v���P�[�V���� �t�H�[�������ׂĕ���ꂽ��ɔ������܂��B���̃C�x���g�́A�ʏ�̏I���ȊO�̕��@�ŃA�v���P�[�V�������I�����ꂽ�Ƃ��ɂ͔������܂���B
    ' UnhandledException: �n���h������Ă��Ȃ���O���A�v���P�[�V�����Ŕ��������Ƃ��ɔ�������C�x���g�ł��B
    ' StartupNextInstance: �P��C���X�^���X �A�v���P�[�V�������N������A���ꂪ���ɃA�N�e�B�u�ł���Ƃ��ɔ������܂��B 
    ' NetworkAvailabilityChanged: �l�b�g���[�N�ڑ����ڑ����ꂽ�Ƃ��A�܂��͐ؒf���ꂽ�Ƃ��ɔ������܂��B
    Partial Friend Class MyApplication

        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            ' EMAB�ɃG���[�𔭍s �K�v�ɉ����ă��[�U�[�Ƀ��b�Z�[�W�̕\���Ȃǂ��s��
            'Microsoft.ApplicationBlocks.ExceptionManagement.ExceptionManager.Publish(e.Exception)
            Itis.ApplicationBlocks.ExceptionManagement.UnTrappedExceptionManager.Publish(e.Exception)
        End Sub
    End Class

End Namespace

