import { Outlet, RouterProvider, createBrowserRouter } from "react-router-dom";

import { ThemeProvider } from "@mui/material/styles";
import { StyledEngineProvider } from "@mui/material";
import NavigationScroll from "layouts/NavigationScroll";
import MainWrapper from "layouts/MainWrapper";
import AuthForm from "features/Auth/AuthForm";
import PrivateRoute from "./PrivateRoute";
import PublicRoute from "./PublicRoute";
import themes from "themes";
import MainLayout from "layouts/MainLayout";
import ServiceListView from "features/Service/ServiceListView"
import ServiceAddEditView from "features/Service/ServiceAddEditView"

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
              { path: "Service", element: <ServiceListView /> },
              { path: "Service/addedit", element: <ServiceAddEditView /> },
            ]
          }]
      },
      {
        element: <PublicRoute />,
        children: [

          { path: "/login", element: <AuthForm /> },
          { path: "/", element: <AuthForm /> },

        ]
      },
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
