#!/usr/bin/env groovy

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
		print "SCM Checkout"
		checkout scm
	  }
    } 
	stage('Nuget Restore') {
	  steps {
	    print "Restoring Nuget Packages on sln"
        bat "nuget restore $ENV:WORKSPACE\\CGP.sln"
	  }
	}    
	stage('Start Sonarqube Scanner') {
	  steps {
		print "Sonarqube Analysis Start"
        withSonarQubeEnv('sonarqube') {
          bat "${env.MSBUILD_SONAR_HOME}\\SonarScanner.MSBuild.exe begin /k:${env.key}"	
		}
	  }
	}
	stage('Build Solution') {
      steps {
	    print "Building Solution"
        bat "msbuild $ENV:WORKSPACE\\CGP.sln /p:DeployOnBuild=true /p:Configuration=Release /p:WebPublishMethod=FileSystem /p:SkipInvalidConfigurations=true /p:DeployDefaultTarget=WebPublish /p:PublishUrl=$ENV:WORKSPACE\\Build_Artifacts_Jenkins"
      }
    }    
	stage('SonarQube Analysis') {
      steps {
		print "Sonarqube Analysis End"
        withSonarQubeEnv('sonarqube') {
          bat "${env.MSBUILD_SONAR_HOME}\\SonarScanner.MSBuild.exe end"
        }
      }
    }	    
	stage('Remove PDB Files') {
      steps {
	    print "Removed .Pdb Files"
        powershell '''
		Get-ChildItem $ENV:WORKSPACE\\Build_Artifacts_Jenkins *.pdb -Recurse | foreach { Remove-Item -Path $_.FullName -Force }
        Remove-Item -Path $ENV:WORKSPACE\\Build_Artifacts_Jenkins\\bin\\roslyn -Recurse -Force
		md -path $ENV:WORKSPACE\\Build_Package.$ENV:BUILD_NUMBER
		'''
	  }
	}	    
	stage('Publish Artifacts To Jenkins Dashboard') {
	  steps{
		archiveArtifacts artifacts: "Build_Package.$ENV:BUILD_NUMBER",  onlyIfSuccessful: true
	  }
	}    
  }
}
