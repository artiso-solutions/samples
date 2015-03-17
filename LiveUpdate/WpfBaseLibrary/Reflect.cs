using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfBaseLibrary
{
   using System.Linq.Expressions;
   public static class Reflect
   {
      public static System.Reflection.MemberInfo GetMember(Expression<Action> expression)
      {
         if (expression == null)
         {
            throw new System.ArgumentNullException(Reflect.GetMember<Expression<Action>>(() => expression).Name);
         }
         return Reflect.GetMemberInfo(expression);
      }
      public static System.Reflection.MemberInfo GetMember<T>(Expression<Func<T>> expression)
      {
         if (expression == null)
         {
            throw new System.ArgumentNullException(Reflect.GetMember<Expression<Func<T>>>(() => expression).Name);
         }
         return Reflect.GetMemberInfo(expression);
      }
      public static System.Reflection.MethodInfo GetMethod(Expression<Action> expression)
      {
         System.Reflection.MethodInfo methodInfo = Reflect.GetMember(expression) as System.Reflection.MethodInfo;
         if (methodInfo == null)
         {
            throw new System.ArgumentException("Not a method call expression", Reflect.GetMember<Expression<Action>>(() => expression).Name);
         }
         return methodInfo;
      }
      public static System.Reflection.PropertyInfo GetProperty<T>(Expression<Func<T>> expression)
      {
         System.Reflection.PropertyInfo propertyInfo = Reflect.GetMember<T>(expression) as System.Reflection.PropertyInfo;
         if (propertyInfo == null)
         {
            throw new System.ArgumentException("Not a property expression", Reflect.GetMember<Expression<Func<T>>>(() => expression).Name);
         }
         return propertyInfo;
      }
      public static System.Reflection.FieldInfo GetField<T>(Expression<Func<T>> expression)
      {
         System.Reflection.FieldInfo fieldInfo = Reflect.GetMember<T>(expression) as System.Reflection.FieldInfo;
         if (fieldInfo == null)
         {
            throw new System.ArgumentException("Not a field expression", Reflect.GetMember<Expression<Func<T>>>(() => expression).Name);
         }
         return fieldInfo;
      }
      internal static System.Reflection.MemberInfo GetMemberInfo(LambdaExpression lambda)
      {
         if (lambda == null)
         {
            throw new System.ArgumentNullException(Reflect.GetMember<LambdaExpression>(() => lambda).Name);
         }
         MemberExpression memberExpression = null;
         if (lambda.Body.NodeType == ExpressionType.Convert)
         {
            memberExpression = (((UnaryExpression)lambda.Body).Operand as MemberExpression);
         }
         else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
         {
            memberExpression = (lambda.Body as MemberExpression);
         }
         else if (lambda.Body.NodeType == ExpressionType.Call)
         {
            return ((MethodCallExpression)lambda.Body).Method;
         }
         if (memberExpression == null)
         {
            throw new System.ArgumentException("Not a member access", Reflect.GetMember<LambdaExpression>(() => lambda).Name);
         }
         return memberExpression.Member;
      }
   }
}
