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
        msbuild $ENV:WORKSPACE\\CGP.sln `
        /p:DeployOnBuild=true `
        /p:Configuration=Release `
        /p:WebPublishMethod=FileSystem `
        /p:SkipInvalidConfigurations=true `
        /p:PublishUrl=${WORKSPACE}\\Build_Artifacts_Jenkins `
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
        Get-ChildItem -Path ${WORKSPACE}\\Build_Artifacts_Jenkins *.pdb -Recurse | foreach { Remove-Item -Path $_.FullName -Force }
        Remove-Item -Path ${WORKSPACE}\\Build_Artifacts_Jenkins\\bin\\roslyn -Recurse -Force
        md -path ${WORKSPACE}\\Build_Package
        '''
      }
    }
    stage('Archive Artifacts') {
      steps {
        powershell '''
        Compress-Archive -Path ${WORKSPACE}\\Build_Package `
        -DestinationPath ${WORKSPACE}\\Build_Package\\MyPackage.${BUILD_NUMBER}.zip
        '''
      }
    }
    stage('Publish Artifacts To Jenkins Dashboard') {
      steps{
        archiveArtifacts artifacts: "MyPackage.${BUILD_NUMBER}.zip",  onlyIfSuccessful: true
      }
    }
  }
}
