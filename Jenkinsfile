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
    PACKAGE_VERSION="${BUILD_NUMBER}.zip"
  }

  stages {
    stage('Checkout') {
      steps {
        checkout scm
      }
    }
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
        /p:PublishUrl=Build_Artifacts_Jenkins `
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
    stage('Remove PDB Files') {
      steps {
        powershell '''
        Get-ChildItem -Path Build_Artifacts_Jenkins *.pdb -Recurse | foreach { Remove-Item -Path $_.FullName -Force }
        Remove-Item -Path Build_Artifacts_Jenkins\\bin\\roslyn -Recurse -Force
        #if (!(test-path -path Artifacts)) {new-item -path Artifacts -itemtype directory}
        '''
      }
    }
    stage('Archive Artifacts') {
      steps {
        script {
          zip zipFile: $PACKAGE_VERSION, archive: true, dir: 'Build_Artifacts_Jenkins'
        }

      }
    }
    stage('Publish Artifacts To Jenkins Dashboard') {
      steps{
        archiveArtifacts artifacts: $PACKAGE_VERSION, fingerprint: true
      }
    }
  }
}
