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
        bat "C:\\nuget\\nuget.exe restore $ENV:WORKSPACE\\CGP.sln"
      }
    }
    stage('Start Sonarqube Scanner') {
      steps {
        withSonarQubeEnv('sonarqube') {
          bat "${env.MSBUILD_SONAR_HOME}\\SonarScanner.MSBuild.exe begin /k:${env.key}"
        }
      }
    }
    stage('Build Solution') {
      steps {
        bat "msbuild $ENV:WORKSPACE\\CGP.sln /p:DeployOnBuild=true /p:Configuration=Release /p:WebPublishMethod=FileSystem /p:SkipInvalidConfigurations=true /p:DeployDefaultTarget=WebPublish /p:PublishUrl=$ENV:WORKSPACE\\Build_Artifacts_Jenkins"
      }
    }
    stage('SonarQube Analysis') {
      steps {
        withSonarQubeEnv('sonarqube') {
          bat "${env.MSBUILD_SONAR_HOME}\\SonarScanner.MSBuild.exe end"
        }
      }
    }
    stage('Publish Artifacts To Jenkins Dashboard') {
      steps{
        archiveArtifacts artifacts: "Build_Package.$ENV:BUILD_NUMBER",  onlyIfSuccessful: true
      }
    }
  }
}
