pipeline {
  agent any

  options {
    timeout(time: 1, unit: 'HOURS')
    timestamps()
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
        bat "msbuild CGP.sln /p:DeployOnBuild=true /p:Configuration=Release /p:WebPublishMethod=FileSystem /p:SkipInvalidConfigurations=true /p:DeployDefaultTarget=WebPublish /p:PublishUrl=${WORKSPACE}\\Build_Artifacts_Jenkins"
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
        Remove-Item -Path ${WORKSPACE}\\Build_Artifacts_Jenkins\\bin\\roslyn -Recurse -Force
        md -path ${WORKSPACE}\\Build_Package.${BUILD_NUMBER}
        '''
      }
    }
    stage('Publish Artifacts To Jenkins Dashboard') {
      steps{
        archiveArtifacts artifacts: "Build_Package.${BUILD_NUMBER}",  onlyIfSuccessful: true
      }
    }
  }
}
