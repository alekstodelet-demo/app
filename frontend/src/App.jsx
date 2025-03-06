import { Outlet, RouterProvider, createBrowserRouter } from "react-router-dom";

import { ThemeProvider } from "@mui/material/styles";
import { CssBaseline, StyledEngineProvider } from "@mui/material";
import NavigationScroll from "layouts/NavigationScroll";
import MainWrapper from "layouts/MainWrapper";
import S_PlaceHolderTypeAddEditView from "features/S_PlaceHolderType/S_PlaceHolderTypeAddEditView";
import S_PlaceHolderTypeListView from "features/S_PlaceHolderType/S_PlaceHolderTypeListView";
import S_TemplateDocumentPlaceholderAddEditView
  from "features/S_TemplateDocumentPlaceholder/S_TemplateDocumentPlaceholderAddEditView";
import S_TemplateDocumentPlaceholderListView
  from "features/S_TemplateDocumentPlaceholder/S_TemplateDocumentPlaceholderListView";
import S_QueryAddEditView from "features/S_Query/S_QueryAddEditView";
import S_QueryListView from "features/S_Query/S_QueryListView";
import S_QueriesDocumentTemplateAddEditView
  from "features/S_QueriesDocumentTemplate/S_QueriesDocumentTemplateAddEditView";
import S_QueriesDocumentTemplateListView from "features/S_QueriesDocumentTemplate/S_QueriesDocumentTemplateListView";
import S_DocumentTemplateAddEditView from "features/S_DocumentTemplate/S_DocumentTemplateAddEditView";
import S_DocumentTemplateListView from "features/S_DocumentTemplate/S_DocumentTemplateListView";
import LanguageAddEditView from "features/Language/LanguageAddEditView";
import LanguageListView from "features/Language/LanguageListView";
import S_DocumentTemplateTypeAddEditView from "features/S_DocumentTemplateType/S_DocumentTemplateTypeAddEditView";
import S_DocumentTemplateTypeListView from "features/S_DocumentTemplateType/S_DocumentTemplateTypeListView";
import SignIn from "features/Auth/SingIn";
import ForgotPassword from "features/Auth/forgotPassword";
import PrivateRoute from "./PrivateRoute";
import PublicRoute from "./PublicRoute";
import themes from "themes";
import MainLayout from "layouts/MainLayout";

const router = createBrowserRouter([
  {
    element: (
      <MainWrapper>
        <Outlet />
      </MainWrapper>
    ),
    children: [
      {
        element: <PrivateRoute />,
        children: [
          {
            element: <MainLayout />,
            path: "/user",
            children: [
              { path: "S_PlaceHolderType", element: <S_PlaceHolderTypeListView /> },
              { path: "S_PlaceHolderType/addedit", element: <S_PlaceHolderTypeAddEditView /> },
              { path: "S_TemplateDocumentPlaceholder", element: <S_TemplateDocumentPlaceholderListView /> },
              { path: "S_TemplateDocumentPlaceholder/addedit", element: <S_TemplateDocumentPlaceholderAddEditView /> },
              { path: "S_Query", element: <S_QueryListView /> },
              { path: "S_Query/addedit", element: <S_QueryAddEditView /> },
              { path: "S_QueriesDocumentTemplate", element: <S_QueriesDocumentTemplateListView /> },
              { path: "S_QueriesDocumentTemplate/addedit", element: <S_QueriesDocumentTemplateAddEditView /> },
              { path: "S_DocumentTemplate", element: <S_DocumentTemplateListView /> },
              { path: "S_DocumentTemplate/addedit", element: <S_DocumentTemplateAddEditView /> },
              { path: "Language", element: <LanguageListView /> },
              { path: "Language/addedit", element: <LanguageAddEditView /> },
              { path: "S_DocumentTemplateType", element: <S_DocumentTemplateTypeListView /> },
              { path: "S_DocumentTemplateType/addedit", element: <S_DocumentTemplateTypeAddEditView /> },
            ]
          }]
      },
      {
        element: <PublicRoute />,
        children: [

          { path: "/login", element: <SignIn /> },
          { path: "/", element: <SignIn /> },
          { path: "/forgotPassword", element: <ForgotPassword /> },

        ]
      },

      // { path: "Login", element: <AuthLogin /> },
      // { path: "/login", element: <SignIn /> },
      // { path: "/", element: <SignIn /> },
      // { path: "/forgotPassword", element: <ForgotPassword /> },
      { path: "error-404", element: <div></div> },
      { path: "access-denied", element: <div></div> },
      { path: "*", element: <div>not founded</div> }
    ]
  }
]);

const App = () => {
  return <StyledEngineProvider injectFirst>
    <ThemeProvider theme={themes(null)}>
      <NavigationScroll>
        <RouterProvider router={router} />
      </NavigationScroll>
    </ThemeProvider>
  </StyledEngineProvider>;
};

export default App;
