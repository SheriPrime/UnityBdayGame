#!/bin/bash

# Go to your Unity project
cd ~/Documents/Unity/wissgame || exit

echo "📂 Current directory: $(pwd)"

# Show git status
echo "🔍 Checking changes..."
git status

# Ask for commit message
echo "💬 Enter commit message:"
read msg

# Add, commit, push
git add .
git commit -m "$msg"
git push

echo "✅ Done! Your code is now on GitHub 😎"

