pipeline {
  agent any

  options {
    timeout(time: 1, unit: 'HOURS')
    timestamps()
    disableConcurrentBuilds()
    buildDiscarder(logRotator(numToKeepStr: '60'))
  }

  environment{
    MSBUILD_SONAR_HOME = tool 'SonarScanner'
    key = "jenkins"
  }

  stages {
    stage('Nuget Restore') {
      steps {
        bat "nuget restore CGP.sln"
      }
    }
    stage('Start Sonarqube Scanner') {
      steps {
        withSonarQubeEnv('sonarqube') {
          bat "${MSBUILD_SONAR_HOME}\\SonarScanner.MSBuild.exe begin /k:${key}"
        }
      }
    }
    stage('Build Solution') {
      steps {
        powershell '''
        msbuild CGP.sln `
        /p:DeployOnBuild=true `
        /p:Configuration=Release `
        /p:WebPublishMethod=FileSystem `
        /p:SkipInvalidConfigurations=true `
        /p:PublishUrl=$ENV:WORKSPACE\\Build_Artifacts_Jenkins `
        /p:DeployDefaultTarget=WebPublish
        '''
      }
    }
    stage('SonarQube Analysis') {
      steps {
        withSonarQubeEnv('sonarqube') {
          bat "${MSBUILD_SONAR_HOME}\\SonarScanner.MSBuild.exe end"
        }
      }
    }
    stage('Remove .PDB Files') {
      steps {
        powershell '''
        Get-ChildItem -Path $ENV:WORKSPACE\\Build_Artifacts_Jenkins *.pdb -Recurse | foreach { Remove-Item -Path $_.FullName -Force }
        Remove-Item -Path Build_Artifacts_Jenkins\\bin\\roslyn -Recurse -Force
        if (!(test-path -path $ENV:WORKSPACE\\Artifacts)) {new-item -path $ENV:WORKSPACE\\Artifacts -itemtype directory}
        Copy-Item -Path $ENV:WORKSPACE\\Build_Artifacts_Jenkins\\* -Destination $ENV:WORKSPACE\\Artifacts -Recurse
        '''
      }
    }
    stage('Archive Artifacts') {
      steps {
        powershell '''
        Compress-Archive -Path $ENV:WORKSPACE\\Artifacts\\* `
        -DestinationPath $ENV:WORKSPACE\\Artifacts\\Package.$ENV:BUILD_NUMBER.zip
        '''
      }
    }
    stage('Publish Artifacts') {
      steps{
        archiveArtifacts artifacts: '$ENV:WORKSPACE\\Artifacts\\Package.$ENV:BUILD_NUMBER.zip', fingerprint: true
      }
    }
  }
}
