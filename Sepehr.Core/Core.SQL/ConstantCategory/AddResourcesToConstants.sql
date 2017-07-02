Begin Transaction;
Begin try
declare @categoryId int,@GeneralConfigId int
  IF Exists (select * from core.ConstantCategories where Name='resources')
  set @categoryId =(select ID from core.ConstantCategories where Name='resources')
  ELSE
  Begin
  INSERT into  core.ConstantCategories(Name)values('Resources')
   set @categoryId= (select SCOPE_IDENTITY())
  END

  IF NOT Exists (select * from core.ConstantCategories where Name='GeneralConfig')
  set @GeneralConfigId=(select ID from core.ConstantCategories where Name='GeneralConfig')
  else
  Begin
  INSERT into  core.ConstantCategories(Name)values('GeneralConfig')
  set @GeneralConfigId= (select SCOPE_IDENTITY())
  END

  IF NOT Exists (select * from core.ConstantCategories where Name='DefaultLanguage')
  Begin
  INSERT into  core.ConstantCategories(Name)values('DefaultLanguage')
  END
IF NOT Exists (select * from core.Constants where [Key]= N'Delete' and [Culture] = N'fa-IR')
 begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'حذف', N'Delete', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'Edit' and [Culture] = N'fa-IR')
 begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'ويرايش', N'Edit', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'Help' and [Culture] = N'fa-IR')
 begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'راهنما', N'Help', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'HelpImageUrl' and [Culture] = N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Areas/Core/Content/images/help.png', N'HelpImageUrl', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'HelpUrl' and [Culture] = N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Areas/Core/Home/CreateHelpView', N'HelpUrl', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]=  N'Insert' and [Culture] = N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'جديد', N'Insert', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'Refresh' and [Culture] = N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'به روز رساني', N'Refresh', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'Delete' and [Culture] = N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Delete', N'Delete', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'Edit' and [Culture] = N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Edit', N'Edit', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'Help' and [Culture] = N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Help', N'Help', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'HelpImageUrl' and [Culture] = N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Areas/Core/Content/images/help.png', N'HelpImageUrl', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'HelpUrl' and [Culture] = N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Areas/Core/Home/CreateHelpView', N'HelpUrl', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'Insert' and [Culture] = N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Insert', N'Insert', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'Refresh' and [Culture] = N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Refresh', N'Refresh', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'ApplicationFaild' and [Culture] = N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'سيستم با خطا مواجه شده است', N'ApplicationFaild', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'ConfirmPasswordWasNotMatched' and [Culture] = N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'تاييد كلمه عبور صحيح نمي باشد', N'ConfirmPasswordWasNotMatched', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'DomainNotDefined' and [Culture] = N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'دامنه يافت نشد', N'DomainNotDefined', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'IncorrectPassword' and [Culture] = N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'كلمه عبور صحيح نمي باشد', N'IncorrectPassword', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]=  N'IncorrectSecurityCode' and [Culture] = N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'كد امنيتي نادرست است', N'IncorrectSecurityCode', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'IncorrectUserNameOrPassword' and [Culture] = N'fa-IR' )
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'كلمه عبور يا نام كاربري نادرست است', N'IncorrectUserNameOrPassword', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'NameCannotContainsWhiteSpace' and [Culture] = N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'نام شي نمي تواند فاصله خالي داشته باشد', N'NameCannotContainsWhiteSpace', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'NameCantBeEmpty' and [Culture]= N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'نام نمي تواند خالي باشد', N'NameCantBeEmpty', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]=  N'NoRoleIsNotAllowed' and [Culture]= N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'هيچ نقشي مجاز نيست', N'NoRoleIsNotAllowed', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'PasswordIsNotValid' and [Culture]= N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'كلمه عبور بايد بيش از 5 كاراكتر  و  شامل حروف و ارقام باشد', N'PasswordIsNotValid', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'URLCanNotBeEmpty' and [Culture]= N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'آدرس نمي تواند خالي باشد', N'URLCanNotBeEmpty', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'UserIsNotActive' and [Culture]= N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'كاربر مورد نظر غيرفعال است', N'UserIsNotActive', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'UserIsNotExistInDomain' and [Culture]= N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'كاربر در دامين وجود ندارد', N'UserIsNotExistInDomain', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'UserIsNotInPermittedRole' and [Culture]= N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'كاربر در نقش مجاز نيست', N'UserIsNotInPermittedRole', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'ApplicationFaild' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Error Accured in Application', N'ApplicationFaild', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'ConfirmPasswordWasNotMatched' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Confirm Password Was NotMatched', N'ConfirmPasswordWasNotMatched', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'DomainNotDefined' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Domain Not Defined', N'DomainNotDefined', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'IncorrectPassword' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Incorrect Password', N'IncorrectPassword', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'IncorrectSecurityCode' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Incorrect Security Code', N'IncorrectSecurityCode', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'IncorrectUserNameOrPassword' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Incorrect UserName Or Password', N'IncorrectUserNameOrPassword', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'NameCannotContainsWhiteSpace' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Name Cannot Contains WhiteSpace', N'NameCannotContainsWhiteSpace', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'NameCantBeEmpty' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Name Cant Be Empty', N'NameCantBeEmpty', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'NoRoleIsNotAllowed' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'No Role Is Not Allowed', N'NoRoleIsNotAllowed', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'PasswordIsNotValid' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Password Must be More than 5 charactors (contains digits and charactors)', N'PasswordIsNotValid', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'URLCanNotBeEmpty' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'URL Can Not Be Empty', N'URLCanNotBeEmpty', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'UserIsNotActive' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'User Is Not Active', N'UserIsNotActive', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'UserIsNotExistInDomain'  and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'User Is Not Exist In Domain', N'UserIsNotExistInDomain', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'UserIsNotInPermittedRole'  and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'User Hasn''t Proper Permission', N'UserIsNotInPermittedRole', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'ChangePasswordWasSuccessFull'  and [Culture]= N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'تغيير كلمه عبور با موفقيت انجام شد', N'ChangePasswordWasSuccessFull', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'FillTheField'  and [Culture]= N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'فيلد موردنظر را پر نماييد', N'FillTheField', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'PasswordMustBeContainCharactersAndDigits'  and [Culture]= N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'كلمه عبور بايد شامل حروف و اعداد باشد', N'PasswordMustBeContainCharactersAndDigits', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'PasswordMustBeMoreThan5Character'  and [Culture]= N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'كلمه عبور بايد بيش از 5 كاراكتر باشد', N'PasswordMustBeMoreThan5Character', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'ThereIsntAnyData'  and [Culture]= N'fa-IR')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'داده اي وجود ندارد', N'ThereIsntAnyData', N'fa-IR')
end
IF NOT Exists (select * from core.Constants where [Key]= N'ChangePasswordWasSuccessFull' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Change Password Was SuccessFull', N'ChangePasswordWasSuccessFull', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'FillTheField' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Required', N'FillTheField', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'PasswordMustBeContainCharactersAndDigits' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Password Must Be Contain Characters And Digits', N'PasswordMustBeContainCharactersAndDigits', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'PasswordMustBeMoreThan5Character' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'Password Must Be More Than 5 Character', N'PasswordMustBeMoreThan5Character', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]=  N'ThereIsntAnyData' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @categoryId, N'There Isnt Any Information', N'ThereIsntAnyData', N'en-US')
end
IF NOT Exists (select * from core.Constants where [Key]= N'AuthCookieExpireDays' and [Culture]= N'en-US')
begin
INSERT [core].[Constants] ( [ConstantCategoryID], [Value], [Key], [Culture]) VALUES ( @GeneralConfigId, '0.020833333333333332', N'AuthCookieExpireDays', N'en-US')
end


END TRY
BEGIN CATCH
    SELECT 
        ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0
    COMMIT TRANSACTION;
GO

select count(*) from core.Constants
