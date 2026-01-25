<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" indent="yes"/>

  <xsl:param name="CurrentDate" />

  <xsl:template match="/">
    <LegacyImportRequest>
      <Header>
        <SourceSystem>PAYMENT_GATEWAY</SourceSystem>
        <GeneratedAt><xsl:value-of select="$CurrentDate"/></GeneratedAt>
      </Header>
      <Body>
        <ReferenceNumber>
          <xsl:value-of select="BankingTransaction/TransactionId"/>
        </ReferenceNumber>
        
        <MerchantCode>
          <xsl:value-of select="BankingTransaction/Remitter"/>
        </MerchantCode>
        
        <Value>
          <xsl:value-of select="BankingTransaction/Amount"/>
        </Value>
        
        <Currency>
          <xsl:value-of select="BankingTransaction/CurrencyCode"/>
        </Currency>
      </Body>
    </LegacyImportRequest>
  </xsl:template>
</xsl:stylesheet>