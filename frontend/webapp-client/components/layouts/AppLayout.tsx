import Head from "next/head";

const AppLayout = ({ children }: any) => {
  return (
    <div>
      <Head>
        <title>Boring Finances</title>
      </Head>
      <main>{children}</main>
    </div>
  );
};

export default AppLayout;
